; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "RingSoft WPF Data Entry Controls Demo"
#define MyAppVersion "2.00.03"
#define MyAppPublisher "RingSoft"
#define MyAppURL "http://www.ringsoft.site/"
#define MyAppExeName "RingSoft.DataEntryControls.NorthwindApp.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application. Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{ecfa3292-3aa4-459f-851f-f28e94bf73d8}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\{#MyAppName}
DisableDirPage=no
DisableProgramGroupPage=no
; Remove the following line to run in administrative install mode (install for all users.)
PrivilegesRequired=lowest
PrivilegesRequiredOverridesAllowed=dialog
OutputDir=C:\Inno Install Output
OutputBaseFilename=RingSoft.WPFDataEntryControlsDemoApp
Compression=lzma
SolidCompression=yes
WizardStyle=modern

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "C:\Users\petem\source\repos\RingSoft\RingSoft.DataEntryControls\RingSoft.DataEntryControls.NorthwindApp\bin\Release\net8.0-windows7.0\*"; Excludes: "*.xml, *.sqlite"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "C:\Users\petem\source\repos\RingSoft\RingSoft.DataEntryControls\RingSoft.DataEntryControls.NorthwindApp\bin\Release\net8.0-windows7.0\RSDEC_Northwind.sqlite"; DestDir: "{commonappdata}\RingSoft\DataEntryNorthwindDemoApp\"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

