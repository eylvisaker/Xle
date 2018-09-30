#!/bin/bash

sourceDir=$1
remoteHost=$FTP_REMOTE_HOST
username=$FTP_USERNAME
password=$FTP_PASSWORD

lotadest="LegacyOfTheAncients"
lobdest="LegendOfBlacksilver"

if [[ ! -z $FTP_DEST ]]; then
  lotadest=$FTP_DEST/$lotadest
  lobdest=$FTP_DEST/$lobdest
fi

echo "Uploading to ftp://$FTP_REMOTE_HOST/$dest"
echo "Logging in as $FTP_USERNAME"

ftp-upload -h $FTP_REMOTE_HOST -u $FTP_USERNAME --password $FTP_PASSWORD -d $lotadest $1/LegacyOfTheAncients*.zip
ftp-upload -h $FTP_REMOTE_HOST -u $FTP_USERNAME --password $FTP_PASSWORD -d $lobdest $1/LegendOfBlacksilver*.zip
