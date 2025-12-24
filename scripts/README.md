# Note on Scripts

This directory contains utility and setup scripts used for initializing and managing the Mirama microservice backend environment. Below is a brief description of each script file in this directory.

## Script List

- **init_db.sql**  
  Runs during the setup of Docker Compose to create secure schemas for each microservice, allowing them to access only their designated schema. This approach avoids the need to host multiple databases during development.  
  **Note:** For actual production deployments, it is recommended to use the db-per-service pattern, where each service has its own dedicated database rather than just being separated by schemas and user permissions.
