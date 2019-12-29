#!/bin/sh
#
# Copies core script for Unity package.

find_cs_src () {
    cd "$1"
    find * -name "*.cs" | grep -Ev "bin/|obj/|csproj"
}

for script in $(find_cs_src "$1")
do
    new_path="$2/${script}"
    old_path="$1/${script}"
    new_dir="$(dirname ${new_path})"
    mkdir -p "${new_dir}"
    cp "${old_path}" "${new_path}"
done