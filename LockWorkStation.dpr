program LockWorkStation;

uses
  Forms,
  uLock in 'uLock.pas' {ufLock};

{$R *.RES}

begin
  Application.Initialize;
  Application.Title := 'Lock Workstation';
  Application.CreateForm(TufLock, ufLock);
  Application.Run;
end.
