#!/bin/sh
#
# Outputs changelog since previous tag.

changes=$(git log $1..$2 --grep="pull request" --pretty=format:%b)

echo "$changes" | tac