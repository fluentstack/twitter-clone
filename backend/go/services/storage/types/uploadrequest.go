package types

type UploadRequest struct {
	MediaType *Category `json:"mediaType" validate:"required"`
}
