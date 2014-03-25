using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Ninject;
using Ninject.Parameters;
using Org.InCommon.InCert.Engine.Extensions;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.ContentControlWrappers;
using Org.InCommon.InCert.Engine.UserInterface.ContentWrappers.LinkWrappers;
using Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.HyperlinkModels;
using log4net;

namespace Org.InCommon.InCert.Engine.UserInterface.Dialogs.Models.ContentModels
{
    class TextContentModel : AbstractContentModel
    {
        private static readonly ILog Log = Logger.Create();

        public TextContentModel(AbstractModel parentModel)
            : base(parentModel)
        {
            
        }

        

        public override T LoadContent<T>(AbstractContentWrapper wrapper)
        {
            try
            {
                var result = new TextBlock();
                var content = AddLinksToText(wrapper.GetText(), wrapper.GetLinks());
                if (content == null)
                    return default(T);

                content.Name = "MainText";
                result.Inlines.Add(content);
                result.Name = "Content";

                InitializeBindings(result);
                InitializeValues(wrapper);
                Content = result;
                return result as T;
            }
            catch (Exception e)
            {
                Log.Warn(e);
                return default(T);
            }
        }

        public Inline AddLinksToText(string text, List<AbstractLink> links)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(text))
                    return new Span(new Run(""));

                if (links == null)
                    return new Span(new Run(text));

                if (!links.Any())
                    return new Span(new Run(text));

                var positions = new List<Tuple<int, AbstractLink>>();

                foreach (var link in links)
                {
                    if (String.IsNullOrWhiteSpace(link.GetText()))
                        continue;

                    var start = text.IndexOf(link.GetText(), 0, StringComparison.InvariantCulture);
                    if (start == -1)
                        continue;

                    positions.Add(new Tuple<int, AbstractLink>(start, link));
                }

                positions.Sort((a, b) => a.Item1.CompareTo(b.Item1));

                var result = new Span();

                var startPosition = 0;
                foreach (var info in positions)
                {
                    var length = info.Item1 - startPosition;
                    if (length < 0)
                    {
                        Log.Warn("The link text '" + info.Item2.GetText() +
                                            "' exists at an invalid position. Check to ensure that it does not overlap other link text.");
                        continue;
                    }

                    var before = text.Substring(startPosition, info.Item1 - startPosition);
                    if (!String.IsNullOrEmpty(before))
                        result.Inlines.Add(new Run(before));

                    var link = GetHyperlinkForWrapper(info.Item2);
                    if (link == null)
                        continue;

                    result.Inlines.Add(link);
                    startPosition = info.Item1 + info.Item2.GetText().Length;
                }

                if (startPosition < text.Length)
                {
                    result.Inlines.Add(new Run(text.Substring(startPosition, text.Length - startPosition)));
                }

                return result;
            }
            catch (Exception e)
            {
                Log.Warn(e);
                return new Span(new Run(text));
            }
        }

        private Hyperlink GetHyperlinkForWrapper(AbstractLink wrapper)
        {
            var kernel = Application.Current.CurrentKernel();

            var model = kernel.Get(
                wrapper.GetPreferredModelType(),
                new ConstructorArgument("parentModel", this)) as AbstractHyperlinkModel;

            if (model == null)
                return null;

            model.LoadContent(wrapper);
            return model.Content as Hyperlink;
        }

    }
}
