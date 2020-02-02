#!/bin/sh
#
# Outputs changelog since previous tag.

get_heading () {
  printf "## [$1] - $(date --rfc-3339=date)"
}

get_body () {
  changes=$(git log $1..$2 --grep="pull request" --pretty=format:%b)
  echo "$changes" | tac | xargs -d "\n" -L 1 printf "* %s\n"
}