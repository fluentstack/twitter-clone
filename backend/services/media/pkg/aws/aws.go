package aws

import (
	"context"
	"log"

	"github.com/aws/aws-sdk-go-v2/config"
	"github.com/aws/aws-sdk-go-v2/feature/s3/manager"
	"github.com/aws/aws-sdk-go-v2/service/dynamodb"
	"github.com/aws/aws-sdk-go-v2/service/s3"
)

var s3client *s3.Client
var s3uploader *manager.Uploader
var ddbClient *dynamodb.Client

func init() {
	cfg, err := config.LoadDefaultConfig(context.TODO())
	if err != nil {
		log.Fatal(err)
	}
	s3client = s3.NewFromConfig(cfg)
	s3uploader = manager.NewUploader(s3client)
	ddbClient = dynamodb.NewFromConfig(cfg)
}

