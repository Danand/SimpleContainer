#!/bin/sh
#
# Releases package for Unity Package Manager.

# FUNCTIONS:
mv_to_prefix () {
  git mv "$1" "$2" 
}

reveal_meta () {
  placeholder_meta="$2/.$1.meta"
  real_meta="$2/$1.meta"
  
  if [[ -f "${placeholder_meta}" ]]; then
    git mv "${placeholder_meta}" "${real_meta}"
  fi
}

# EXECUTION:
set -e

if [[ "$2" == "--local" ]]; then
  local=true
else
  local=false
fi

if [[ "$3" == "--skip-changelog" ]]; then
  skip_changelog=true
else
  skip_changelog=false
fi

tag="$1"
prefix="SimpleContainer.Unity/Assets"
source_branch=$(git rev-parse --abbrev-ref HEAD)
release_branch="release-package-unity/${tag}"

cd "$(dirname "$0")/.."

git tag ${tag} --force

if ! $skip_changelog; then
  tag_previous=$(git tag | grep -v "package" | tail -n 2 | head -n 1)
  source ./scripts/get-changelog.sh
  printf "\n" >> CHANGELOG.md
  get_heading ${tag} >> CHANGELOG.md
  printf "\n" >> CHANGELOG.md
  get_body ${tag_previous} ${tag} >> CHANGELOG.md
  printf "\n" >> CHANGELOG.md
  
  git add CHANGELOG.md
  git commit -m "Update CHANGELOG.md with version ${tag}"
  
  git tag ${tag} --force
fi

mv_to_prefix README.md "${prefix}"
reveal_meta README.md "${prefix}"
mv_to_prefix LICENSE.md "${prefix}"
reveal_meta LICENSE.md "${prefix}"
mv_to_prefix CHANGELOG.md "${prefix}"
reveal_meta CHANGELOG.md "${prefix}"

git rm --cached "SimpleContainer.Unity/Assets/SimpleContainer/SimpleContainer.Core.csproj"
git rm --cached "SimpleContainer.Unity/Assets/SimpleContainer/SimpleContainer.Core.csproj.meta"

git commit -m "Add package details for ${tag}"

git subtree split --prefix="${prefix}" ${tag} --squash -b ${release_branch}
git checkout ${release_branch} --force

root_commit=$(git rev-list --max-parents=0 --abbrev-commit HEAD)

git reset --soft ${root_commit}
git commit --amend -m "Release Unity package ${tag}"

package_tag="${tag}-package-unity"

git tag ${package_tag}

if ! $local; then
  git push origin -u ${release_branch}
  git push origin ${package_tag}
fi

git checkout ${source_branch} --force
git reset --hard HEAD~1
