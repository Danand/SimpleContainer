#!/bin/sh
#
# Releases package for Unity Package Manager.

# FUNCTIONS:
mv_to_prefix () {
  mv "$1" "$2" && git add "$2/$1"
}

# EXECUTION:
if [[ "$2" == "--local" ]]; then
  local=true
else
  local=false
fi

tag="$1"
prefix="SimpleContainer.Unity/Assets"
source_branch=$(git rev-parse --abbrev-ref HEAD)
release_branch="release-package-unity/${tag}"

mv_to_prefix README.md "${prefix}"
mv_to_prefix LICENSE.md "${prefix}"
mv_to_prefix CHANGELOG.md "${prefix}"

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
