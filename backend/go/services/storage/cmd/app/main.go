package main

import (
	"log"
	"storage/controllers"
	"storage/handlers"

	"github.com/gofiber/fiber/v2"
)

func main() {
	app := fiber.New()

	app.Use(func(c *fiber.Ctx) error {
		log.Printf("Incoming Request - Method: %s, URL: %s", c.Method(), c.OriginalURL())
		return c.Next()
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
