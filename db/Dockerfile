# Runs init.sql against the sqlserver service. The script is copied in (not mounted)
# so it works from any folder, even ones not shared with Docker Desktop.
FROM mcr.microsoft.com/mssql/server:2022-latest
COPY init.sql /init.sql
