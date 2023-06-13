package controllers

import (
	"io/ioutil"
	"media/types"

	"github.com/gofiber/fiber/v2"
)

func (controller *MediaController) Upload(c *fiber.Ctx) error {
	var request types.UploadRequest
	err := c.BodyParser(&request)
	if err != nil {
		return err
	}
	file, _ := c.FormFile("file")
	fileData, err := file.Open()
	if err != nil {
		return c.Status(fiber.StatusInternalServerError).SendString("Failed to open uploaded file")
	}
	defer fileData.Close()
	fileBytes, err := ioutil.ReadAll(fileData)
	if err != nil {
		return c.Status(fiber.StatusInternalServerError).SendString("Failed to read file contents")
	}	
	metadataId, err := controller.handler.Upload(file.Filename, fileBytes, *request.MediaType)
	if err != nil {
		return c.Status(fiber.StatusInternalServerError).SendString(err.Error())
	}

	return c.Status(fiber.StatusOK).JSON(metadataId)
}