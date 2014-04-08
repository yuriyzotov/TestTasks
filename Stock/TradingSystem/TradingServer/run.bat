SET port 3333

start .\bin\TradingServer.exe %port%
timeout /t 1
start telnet localhost %port%
timeout /t 1
start telnet localhost %port%
timeout /t 1
start telnet localhost %port%
timeout /t 1
start telnet localhost %port%