@ECHO OFF

SET ddfFile=%Temp%.\MakeSecureCab.ddf
IF EXIST "%ddfFile%" (del "%ddfFile%")

ECHO .OPTION EXPLICIT>"%ddfFile%"
ECHO .Set CabinetNameTemplate=%1>>"%ddfFile%"
ECHO. >>"%ddfFile%"
ECHO .Set SourceDir=%2>>"%ddfFile%"
ECHO .Set DiskDirectoryTemplate=CDROM>>"%ddfFile%"
ECHO .Set CompressionType=MSZIP>>"%ddfFile%"
ECHO .Set  UniqueFiles=OFF>>"%ddfFile%"
ECHO .Set Cabinet=on>>"%ddfFile%"
ECHO .Set DiskDirectory1=%3>>"%ddfFile%"
ECHO .Set RptFileName="%Temp%.\makesecurecab.rpt">>"%ddfFile%"
ECHO .Set InfFileName="%Temp%.\makesecurecab.inf">>"%ddfFile%"
ECHO. >>"%ddfFile%"
ECHO "init.xml">>"%ddfFile%"
ECHO "icons\icon.ico">>"%ddfFile%"
makecab /f "%ddfFile%" /v3