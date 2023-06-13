package types

type Category int

const (
	ImageAttachment Category = iota
	VideoAttachment
	Avatar
)

func (category Category) ToString() string {
	switch category {
	case VideoAttachment:
		return "video-attachments"
	case ImageAttachment:
		return "image-attachments"
	case Avatar:
		return "avatars"
	}
	return "misc"
}
