using System;
using System.Net.Cache;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Org.InCommon.InCert.Engine.Engines;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Importables;
using Org.InCommon.InCert.Engine.Results;
using Org.InCommon.InCert.Engine.Results.ControlResults;
using Org.InCommon.InCert.Engine.Results.Errors.General;
using Org.InCommon.InCert.Engine.Utilities;
using Org.InCommon.InCert.Engine.WebServices.Managers;

namespace Org.InCommon.InCert.Engine.Tasks.UserInterface
{
    public class LoadImageFromUri : AbstractTask
    {
        public LoadImageFromUri(IEngine engine)
            : base(engine)
        {
        }

        [PropertyAllowedFromXml]
        public string Uri
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public string ImageKey
        {
            get { return GetDynamicValue(); }
            set { SetDynamicValue(value); }
        }

        [PropertyAllowedFromXml]
        public int Retries { get; set; }

        public override IResult Execute(IResult previousResults)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Uri))
                    throw new Exception("The uri parameter cannot be null or empty");

                var uri = new Uri(Uri, UriKind.RelativeOrAbsolute);
                if (!uri.IsAbsoluteUri)
                {
                    var contentUri = new Uri(Engine.EndpointManager.GetEndpointForFunction(EndPointFunctions.GetContent));
                    uri = new Uri(contentUri, uri);
                }

                var image = GetImageAsynchronous(uri);
                SettingsManager.SetTemporaryObject(
                    ImageKey, image);

                return new NextResult();
            }
            catch (Exception e)
            {
                return new ExceptionOccurred(e);
            }
        }

        private BitmapFrame GetImageAsynchronous(Uri uri)
        {
            var count = 0;
            Exception issue;
            do
            {
                using (var task = new Task<BitmapFrame>(() => GetImageFromUri(uri)))
                {
                    count++;
                    task.Start();
                    task.WaitUntilExited();

                    if (!task.IsFaulted && !task.IsCanceled)
                    {
                        return task.Result;
                    }

                    if (task.IsCanceled)
                    {
                        throw new Exception("Could not retrieve image from uri: task cancelled");
                    }

                    issue =  task.Exception != null
                            ? task.Exception.Flatten()
                            : new Exception(string.Format("An unexpected issue occurred while attempting to retrieve an image from the uri {0}", Uri));
                    
                    UserInterfaceUtilities.WaitForMilliseconds(DateTime.UtcNow, 500);
                }
            } while (count <= Retries);

            throw issue;
        }

        private static BitmapFrame GetImageFromUri(Uri uri)
        {
            var decoder = new PngBitmapDecoder(uri, BitmapCreateOptions.None, BitmapCacheOption.Default);
            var result = decoder.Frames[0];
            return result.GetAsFrozen() as BitmapFrame;
        }

        public override string GetFriendlyName()
        {
            return string.Format("Load image from Uri ({0}); key = {1}", Uri, ImageKey);
        }
    }
}
