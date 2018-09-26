#!/bin/bash

sourceDir=$1
remoteHost=$FTP_REMOTE_HOST
username=$FTP_USERNAME
password=$FTP_PASSWORD
dest=$FTP_DEST/SmokedGB

echo "Uploading to ftp://$FTP_REMOTE_HOST/$dest"
echo "Logging in as $FTP_USERNAME"

ftp-upload -h $FTP_REMOTE_HOST -u $FTP_USERNAME --password $FTP_PASSWORD -d $FTP_DEST $1/*Windows*.zip
