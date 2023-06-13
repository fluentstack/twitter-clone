package aws

import (
	"bytes"
	"context"

	"github.com/aws/aws-sdk-go-v2/aws"
	"github.com/aws/aws-sdk-go-v2/service/s3"
)

func UploadToS3(key, bucketName string, fileData []byte) (string,error) {
	var retryCount int32
	var objectKey string
	for {
		result, err := s3uploader.Upload(context.TODO(), &s3.PutObjectInput{
			Bucket: aws.String(bucketName),
			Key:    aws.String(key),
			Body:   bytes.NewReader(fileData),
		})
		if err != nil && retryCount < MaxRetryAttempts{
			retryCount++
			if retryCount >= MaxRetryAttempts{
				return "", err
			}
		} else{
			objectKey = *result.Key
			break
		}
	}
	return objectKey,nil
}