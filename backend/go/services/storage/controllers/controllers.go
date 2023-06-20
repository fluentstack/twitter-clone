package controllers

import (
	"storage/handlers"

	"github.com/gofiber/fiber/v2"
)

func AddRoutes(app *fiber.App, controller *MediaController) {
	routes := app.Group("/media")
	
	routes.Get("/", func(c *fiber.Ctx) error {
		return c.SendString("Hello, World 👋!")
	})
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
