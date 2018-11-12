DROP DATABASE IF EXISTS tracker_dotnet_dev;
DROP DATABASE IF EXISTS tracker_dotnet_test;

CREATE DATABASE tracker_dotnet_dev;
CREATE DATABASE tracker_dotnet_test;

CREATE USER IF NOT EXISTS 'tracker_dotnet'@'localhost'
  IDENTIFIED BY 'password';
GRANT ALL PRIVILEGES ON tracker_dotnet_dev.* TO 'tracker_dotnet'@'localhost';
GRANT ALL PRIVILEGES ON tracker_dotnet_test.* TO 'tracker_dotnet'@'localhost';