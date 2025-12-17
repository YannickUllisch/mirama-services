-- To not have to deploy a separate Database for each service in this test project
-- but still be able to mimik DB separation per service, we create a separate schema 
-- for each service and ONLY allow its user to use that schema.

-- ####### Auth Service #######
CREATE SCHEMA IF NOT EXISTS auth;

CREATE USER auth_user WITH PASSWORD 'auth_password';

-- Grant user access only to auth schema
GRANT USAGE ON SCHEMA auth TO auth_user;

GRANT CREATE ON SCHEMA auth TO auth_user;

-- Allow the user to create tables in this schema (needed for EF migrations)
GRANT INSERT, SELECT, UPDATE, DELETE ON ALL TABLES IN SCHEMA auth TO auth_user;

-- Make future tables automatically accessible
ALTER DEFAULT PRIVILEGES IN SCHEMA auth
  GRANT SELECT, INSERT, UPDATE, DELETE ON TABLES TO auth_user;


-- ####### Project Service #######
CREATE SCHEMA IF NOT EXISTS project;

CREATE USER project_user WITH PASSWORD 'project_password';

GRANT USAGE ON SCHEMA project TO project_user;

GRANT CREATE ON SCHEMA project TO project_user;

GRANT INSERT, SELECT, UPDATE, DELETE ON ALL TABLES IN SCHEMA project TO project_user;

ALTER DEFAULT PRIVILEGES IN SCHEMA project
  GRANT SELECT, INSERT, UPDATE, DELETE ON TABLES TO project_user;