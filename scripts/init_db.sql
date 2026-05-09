-- Single DB, separate schemas per module.
-- One app user with access to all module schemas.

CREATE SCHEMA IF NOT EXISTS identity;
CREATE SCHEMA IF NOT EXISTS projects;

CREATE USER mirama_user WITH PASSWORD 'mirama_password';

GRANT USAGE ON SCHEMA identity TO mirama_user;
GRANT USAGE ON SCHEMA projects TO mirama_user;

GRANT CREATE ON SCHEMA identity TO mirama_user;
GRANT CREATE ON SCHEMA projects TO mirama_user;

GRANT INSERT, SELECT, UPDATE, DELETE ON ALL TABLES IN SCHEMA identity TO mirama_user;
GRANT INSERT, SELECT, UPDATE, DELETE ON ALL TABLES IN SCHEMA projects TO mirama_user;

ALTER DEFAULT PRIVILEGES IN SCHEMA identity
  GRANT SELECT, INSERT, UPDATE, DELETE ON TABLES TO mirama_user;

ALTER DEFAULT PRIVILEGES IN SCHEMA projects
  GRANT SELECT, INSERT, UPDATE, DELETE ON TABLES TO mirama_user;
