package main

import (
	"log"
	"storage/controllers"
	"storage/handlers"

	"github.com/gofiber/fiber/v2"
)

func main() {
	app := fiber.New()

	// Custom middleware to log incoming requests
	app.Use(func(c *fiber.Ctx) error {
		// Log the incoming request details
		log.Printf("Incoming Request - Method: %s, URL: %s", c.Method(), c.OriginalURL())

		// Proceed to the next middleware or route handler
		return c.Next()
	})

	app.Get("/", func(c *fiber.Ctx) error {
		return c.SendString("Hello, World 👋!")
	})

	// api.media.localhost.com/v1/media?type=avatar GET
	// api.media.localhost.com/v1/media POST
	// api.media.localhost.com/v1/media/{media_id} GET
	// api.media.localhost.com/v1/media/{media_id} DELETE
	// api.media.localhost.com/v1/media/{media_id}/metadata GET
	// api.media.localhost.com/v1/media/{media_id}/metadata PATCH

	mediaHandler := handlers.NewMediaHandler()
	mediaController := controllers.NewMediaController(mediaHandler)
	controllers.AddRoutes(app, mediaController)

	app.Listen(":80")
}
