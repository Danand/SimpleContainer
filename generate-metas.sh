#!/bin/sh
#
# Generates `.meta` files for Unity `.cs` scripts.

generate_guid () {
    head -c 16 /dev/random | xxd -p
}

find_cs_src () {
    find . -name "*.cs"
}

find_dirs () {
    find . -type d -not -path "."
}

cd $1

for script in $(find_cs_src)
do
    meta_path="${script}.meta"
    
    if [ -f "${meta_path}" ]; then
        echo "Meta file is already exists: ${meta_path}"
    else
        guid=$(generate_guid)
        timestamp=$(date +%s%3N)
        meta_content="fileFormatVersion: 2\nguid: ${guid}\ntimeCreated: ${timestamp}\c"
        echo -e "${meta_content}" > "${meta_path}"
        echo "Meta file generated: ${meta_path}"
    fi
done

for dir in $(find_dirs)
do
    meta_path="${dir}.meta"
    
    if [ -f "${meta_path}" ]; then
        echo "Meta file is already exists: ${meta_path}"
    else
        guid=$(generate_guid)
        meta_content="fileFormatVersion: 2\nguid: ${guid}\nfolderAsset: yes\nDefaultImporter:\n  externalObjects: {}\n  userData: \n  assetBundleName: \n  assetBundleVariant: \c"
        echo -e "${meta_content}" > "${meta_path}"
        echo "Meta file generated: ${meta_path}"
    fi
done