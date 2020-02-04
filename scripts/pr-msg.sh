#!/bin/sh
#
# Outputs git log for pull request.

cd "$(dirname "$0")/.."

current_branch=$(git rev-parse --abbrev-ref HEAD)
parent_branch="develop"
log=$(git log --pretty=format:%s ${parent_branch}..${current_branch})

echo "# Proposed changes"
echo "$log" | tac | xargs -d "\n" -L 1 printf "* %s\n"

echo -e "\n# References"
echo "Closes #"