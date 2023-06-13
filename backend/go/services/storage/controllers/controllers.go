package controllers

import (
	"storage/handlers"

	"github.com/gofiber/fiber/v2"
)

func AddRoutes(app *fiber.App, controller *MediaController) {
	routes := app.Group("/api/media")

	routes.Post("/", controller.Upload)
}

type MediaController struct {
	handler *handlers.MediaHandler
}

func NewMediaController(handler *handlers.MediaHandler) *MediaController {
	return &MediaController{
		handler: handler,
	}
}