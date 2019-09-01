set EXE=".\ProtoGenPB"
set DST=".\PB"

del %DST%\*.cs
del %EXE%\*.proto
del %EXE%\*.cs
copy .\*.proto %EXE%

pushd %EXE%
	call _BUILD.cmd
popd

md %DST%
move %EXE%\*.cs %DST%\

del %EXE%\*.proto
del %EXE%\*.cs

pause

