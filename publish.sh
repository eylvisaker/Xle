#!/bin/bash

sourceDir=$1
remoteHost=$FTP_REMOTE_HOST
username=$FTP_USERNAME
password=$FTP_PASSWORD

echo "Uploading to ftp://$FTP_REMOTE_HOST/$dest"
echo "Logging in as $FTP_USERNAME"

ftp-upload -h $FTP_REMOTE_HOST -u $FTP_USERNAME --password $FTP_PASSWORD -d $FTP_DEST/LegacyOfTheAncients $1/LegacyOfTheAncients*.zip
ftp-upload -h $FTP_REMOTE_HOST -u $FTP_USERNAME --password $FTP_PASSWORD -d $FTP_DEST/LegendOfBlacksilver$1/LegendOfBlacksilver*.zip
