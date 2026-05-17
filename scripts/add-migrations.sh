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

declare -A MODULES=(
  ["Identity"]="src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity"
  ["Clients"]="src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients"
  ["PM"]="src/Modules/Mirama.Modules.PM/Mirama.Modules.PM"
)

SUCCESS=()
FAILED=()

for module in "${!MODULES[@]}"; do
  project="${MODULES[$module]}"
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
