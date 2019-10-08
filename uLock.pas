unit uLock;

interface

uses
  Windows, Messages, SysUtils, Classes, Graphics, Controls, Forms, Dialogs,
  StdCtrls, ExtCtrls, jpeg;

type
  TufLock = class(TForm)
    Button1: TButton;
    Timer1: TTimer;
    Timer2: TTimer;
    Image1: TImage;
    procedure Button1Click(Sender: TObject);
    procedure FormCreate(Sender: TObject);
    procedure Timer1Timer(Sender: TObject);
    procedure Timer2Timer(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  ufLock: TufLock;

implementation

{$R *.DFM}



function StartApplicationAndWait(AppName : string; CommandLineParameters : string; Win32App : boolean): Boolean;
var
  ExeName: string;
  StartupInfo: TStartupInfo;
  ProcessInfo: TProcessInformation;
  xresult : double;
begin
   if Win32App then
     ExeName := AppName
   else
     ExeName := AppName + ' ' + CommandLineParameters;

   FillChar(StartupInfo, SizeOf(TStartupInfo), 0);
   with StartupInfo do
     begin
     cb := SizeOf(TStartupInfo);
     dwFlags := STARTF_USESHOWWINDOW or STARTF_FORCEONFEEDBACK;
     wShowWindow := SW_SHOWNORMAL;
     end;

   if Win32App then
     Result := CreateProcess( PChar(AppName), PChar(CommandLineParameters), nil, nil, False,
                              NORMAL_PRIORITY_CLASS, nil, nil, StartupInfo, ProcessInfo)
   else   {Exe Name is passed as 2nd param instead of 1st for 16 bit applications}
     Result := CreateProcess( nil, PChar(ExeName), nil, nil, False,
                             NORMAL_PRIORITY_CLASS, nil, nil, StartupInfo, ProcessInfo);
   if Result then
     xresult := WaitForSingleObject(ProcessInfo.hProcess,INFINITE)
   else
     begin
     xresult := GetLastError;
     MessageDlg('Error ' + FloatToStr(Xresult) + ' calling ' + AppName,mtError,[mbOK],0);
     end;
end;

procedure TufLock.Button1Click(Sender: TObject);
begin
StartApplicationAndWait('rundll32 user32.dll,LockWorkStation','',false);
end;

procedure TufLock.FormCreate(Sender: TObject);
begin
Timer1.Enabled := true;
end;

procedure TufLock.Timer1Timer(Sender: TObject);
begin
 timer1.enabled := false;
 StartApplicationAndWait('rundll32 user32.dll,LockWorkStation','',false);

 Timer2.enabled := true;

end;

procedure TufLock.Timer2Timer(Sender: TObject);
begin
ufLock.Close;
end;

end.
