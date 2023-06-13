FROM golang:latest AS build
WORKDIR /app
COPY go.mod go.sum ./
RUN go mod download
COPY . .
RUN CGO_ENABLED=0 GOOS=linux go build -o storage ./cmd/app/
FROM gcr.io/distroless/base-debian11 AS release
WORKDIR /app 
COPY --from=build /app/storage /app/
EXPOSE 80
ENTRYPOINT ["/app/storage"]