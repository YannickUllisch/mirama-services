#!/usr/bin/env bash
set -euo pipefail

if [[ $# -lt 1 ]]; then
  echo "Usage: $0 <MigrationName>"
  exit 1
fi

MIGRATION_NAME="$1"
REPO_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
SERVICE_ROOT="$REPO_ROOT/MiramaService"
STARTUP="src/Mirama.Api"

MODULE_NAMES=(Identity Clients PM Workspace)
MODULE_PATHS=(
  "src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity"
  "src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients"
  "src/Modules/Mirama.Modules.PM/Mirama.Modules.PM"
  "src/Modules/Mirama.Modules.Workspace/Mirama.Modules.Workspace"
)

SUCCESS=()
FAILED=()

cd "$SERVICE_ROOT"

for i in "${!MODULE_NAMES[@]}"; do
  module="${MODULE_NAMES[$i]}"
  project="${MODULE_PATHS[$i]}"
  echo ""
  echo "==> [$module] Adding migration '$MIGRATION_NAME'..."

  if dotnet ef migrations add "$MIGRATION_NAME" \
    --project "$project" \
    --startup-project "$STARTUP" \
    --prefix-output \
    2>&1; then
    SUCCESS+=("$module")
  else
    echo "    FAILED: $module"
    FAILED+=("$module")
  fi
done

echo ""
echo "=== Results ==="
for m in "${SUCCESS[@]:-}"; do echo "  OK  $m"; done
for m in "${FAILED[@]:-}"; do echo "  FAIL $m"; done

[[ ${#FAILED[@]} -eq 0 ]]
