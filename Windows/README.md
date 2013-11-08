InCommon Personal Certificate Provisioning Tool - Windows Client
================================================================

Overview
--------
This folder contains the source for the Windows version of [InCommon Personal Certificate Provisioning and Application Setup Tool](https://spaces.internet2.edu/x/f66KAQ).

Build Prerequities
---
In order to build this project, you will need, at the least, the following:

- Visual Studio 2012
- Wix Toolset v3.7 ([wix url](http://wix.codeplex.com/releases/view/99514 "WiX Toolset download"))
- A valid code-signing certificate

Project Contents
---
The project consists of the following components (in order of importance):

- Engine: contains the source files for the InCert engine proper.
- DataContracts: contains source files for the engines various WebApi data contracts.
- Bootstrapper: contains the source files that will generate the final bootrapper / installer bundle
- Installer: contains the source files to generate the installer.
- BootstrapperEngine: contains the source files to generate the custom bootstrapper engine.  The bootstrapper engine drives the installation process.
- Elevator: contains the source files for the elevator, a small .exe stub to help launching the engine with elevated privileges.
- Utilities/GenerateFileWrapper: contains the source files to build the GenerateFileWrapper helper utility.  This utility builds .info.xml files, which the engine consumes to help it download files, etc.

Customizing the Project - Pre-Build
---
For the most part, the engine is customized by modifying its various xml files, but there are some customizations that need to be compiled into the various engine components.

These are set in the .build/Incert.targets file.  They are:

- ProductName: sets the compiled product name
- Institution: sets the compiled institution name
- ArchivePath: sets the path to the folder whose contents will be added to settings.cab upon build.
- AppVersion: sets the application version
- IconPath: sets the path to the icon to use when building the various engine components
- HelpUrl: sets the installer's help url
- BackgroundColor: sets the installer's background color
- TextColor: sets the installer's text / foreground color
- LogUploader: specifies tha log uploader that the installer should use to upload logs.  Valid values are:
    - GetBasedLogUploader: uploads logs using GET requests
    - PostBasedLogUploader: uploads logs using POST requests
- LoggingUrl: server API logging endpoint url.
- StartMenuShortcut: if set to 1, adds an engine shortcut to the start menu when installed
- DesktopShortcut: if set to 1, adds an engine shortcut to the desktop menu when installed
- CertificateThumbprint: thumbprint to code-signing cert installed in certificate store that will be used to sign various engine components. Note that this is not required, but the engine is designed not to continue if it detects that it is not signed with a valid code-signing certificate.
- NetFx45DownloadUrl: url to use to download .NET framework installer when building the final bootstrap package.

Note that you may need to close and re-open the project in Visual Studio for these changes to effect.  This is because Visual Studio will cache the values in the Incert.targets file when it is first loaded.
