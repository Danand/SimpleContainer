#!/bin/sh

# Deletes release for Unity Package Manager.

tag="$1"

if [[ "$2" == "--local" ]]; then
  local=true
else
  local=false
fi

git tag --delete ${tag}
git tag --delete ${tag}-package-unity
git branch -D release-package-unity/${tag}

if ! $local; then
  git push origin :refs/tags/${tag}
  git push origin :refs/tags/${tag}-package-unity
  git push origin :refs/heads/release-package-unity/${tag}
fi
