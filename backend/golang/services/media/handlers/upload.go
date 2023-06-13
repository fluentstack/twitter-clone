package handlers

import (
	"media/pkg/aws"
	"media/types"
	"path/filepath"
	"strconv"
	"strings"
	"time"

	"github.com/google/uuid"
)

func (a *MediaHandler) Upload(fileName string, file []byte, fileType types.Category) (string, error) {

	// filename formatting
	fileExtension := filepath.Ext(fileName)
	fileNameOnly := strings.Trim(fileName, fileExtension)
	fileNameOnly = strings.ToLower(fileNameOnly)
	fileNameOnly = strings.ReplaceAll(fileNameOnly, " ", "-")
	formattedFileName := fileNameOnly + "-" + strconv.FormatInt(time.Now().UnixNano(), 10) + fileExtension
	key := fileType.ToString() + "/" + formattedFileName
	
	_, err := aws.UploadToS3(key, "cwm-dotnet-bucket", file)
	if err != nil {
		return "", err
	}
	metaData := types.Metadata{
		Id:       uuid.New().String(),
		Key:      key,
		Category: fileType.ToString(),
	}
	err = aws.PutItemToDynamoDB(metaData, "media-metadata")
	if err != nil {
		return "", err
	}
	return metaData.Id, nil
}
