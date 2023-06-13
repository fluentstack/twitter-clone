package aws

import (
	"context"

	"github.com/aws/aws-sdk-go-v2/aws"
	"github.com/aws/aws-sdk-go-v2/feature/dynamodb/attributevalue"
	"github.com/aws/aws-sdk-go-v2/service/dynamodb"
)

func PutItemToDynamoDB(item interface{}, tableName string) error {
	ddbItem, err := attributevalue.MarshalMap(item)
	if err != nil {
		return err
	}
	input := &dynamodb.PutItemInput{
		Item:      ddbItem,
		TableName: aws.String(tableName),
	}
	_, err = ddbClient.PutItem(context.TODO(), input)
	if err != nil {
		return err
	}
	return nil
}
