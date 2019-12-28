#!/bin/sh
#
# Generates `.meta` files for Unity `.cs` scripts.

generate_guid () {
    head -c 16 /dev/random | xxd -p
}

find_cs_src () {
    find . -name "*.cs" -not -path "./obj/*" -not -path "./bin/*"
}

cd $1

for script in $(find_cs_src)
do
    meta_path="${script}.meta"
    guid=$(generate_guid)
    timestamp=$(date +%s%3N)
    meta_content="fileFormatVersion: 2\nguid: ${guid}\ntimeCreated: ${timestamp}"
    echo -e $meta_content > "${meta_path}"
    echo "Meta file generated: ${meta_path}"
done

# TODO: generate folders meta, here is template.
#fileFormatVersion: 2
#guid: 999f6d192451417409be055c698088a0
#folderAsset: yes
#DefaultImporter:
#  externalObjects: {}
#  userData: 
#  assetBundleName: 
#  assetBundleVariant: 