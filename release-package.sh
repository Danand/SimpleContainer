#!/bin/sh
#
# Releases package for Unity Package Manager.

tag="$1"
source_branch=$(git rev-parse --abbrev-ref HEAD)
release_branch="release-package-unity/${tag}"

git subtree split --prefix=SimpleContainer.Unity/Assets ${tag} --squash -b ${release_branch}
git checkout ${release_branch} --force

root_commit=$(git rev-list --max-parents=0 --abbrev-commit HEAD)

git reset --soft ${root_commit}
git commit --amend -m "Release Unity package ${tag}"

package_tag="${tag}-package-unity"

git tag ${package_tag}
git push origin -u ${release_branch}
git push origin ${package_tag}
git checkout ${source_branch} --force