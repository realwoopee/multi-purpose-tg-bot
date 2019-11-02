echo "$DOCKER_PASSWORD" | docker login -u "$DOCKER_USERNAME" --password-stdin
docker build -t "multi-purpose-tg-bot" .
docker tag "multi-purpose-tg-bot" "$DOCKER_USERNAME/multi-purpose-tg-bot:$TRAVIS_TAG"
docker tag "multi-purpose-tg-bot" "$DOCKER_USERNAME/multi-purpose-tg-bot:latest"
docker push "$DOCKER_USERNAME/multi-purpose-tg-bot"
