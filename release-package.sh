#!/bin/sh
#
# Releases package for Unity Package Manager.

# FUNCTIONS:
mv_to_prefix () {
  mv README.md "$1" && git add "$1"
}

# EXECUTION:
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
git push origin -u ${release_branch}
git push origin ${package_tag}
git checkout ${source_branch} --force