@echo off

echo ==========================
echo BACKUP AUTOMATICO CASSANDRA
echo ==========================

docker exec -it cassandra1 nodetool snapshot

echo.
echo Snapshot realizado correctamente.
echo.

pause