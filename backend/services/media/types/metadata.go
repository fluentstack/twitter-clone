package types

type Metadata struct {
	Id       string `json:"id" dynamodbav:"id"`
	Key      string `json:"key" dynamodbav:"key"`
	Category string `json:"category" dynamodbav:"category"`
}
