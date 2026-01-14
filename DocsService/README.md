# Documentation Service powered by MkDocs

The **DocsProvider** project is designed to set up an **MkDocs-based documentation service** for the Mirama project management platform. Its goal is to provide a simple, centralized way to visualize diagrams, architecture and design decisions for both the backend and frontend of the platform.

## Purpose

- Host and organize project documentation using [MkDocs](https://www.mkdocs.org/).
- Provide clear diagrams and architecture overviews.
- Document key design decisions and workflows.
- Serve as a single reference for both backend and frontend developers.

---

## How to Use Locally

You can set up DocsProvider locally by following these steps:

1. **Navigate to the project directory**

   ```bash
   cd DocsService
   ````

2. Create a Python virtual environment

    ```bash
    python3 -m venv .venv
    ````

3. Activate the virtual environment (on MacOS/Linux)

    ```bash
    source .venv/bin/activate
    ````

4. Install dependencies

    ```bash
    pip install --upgrade pip
    pip install -r requirements.txt
    ````

5. Run the MkDocs development server

   ```bash
   cd DocsService
   ````

6. Open your browser

    Visit <http://127.0.0.1:8000/>
    to view the documentation locally.

---

## Requirements

To use DocsProvider, ensure you have the following installed:

- Python 3.10+ (3.12 recommended)
- pip (latest version recommended)

## Future Plans

- Expand documentation to include additional personal projects.
- Include CI/CD integration for automatic documentation updates.
- Support for multi-repo aggregation for large-scale projects.
