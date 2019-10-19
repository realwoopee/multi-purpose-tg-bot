echo "$DOCKER_PASSWORD" | docker login -u "$DOCKER_USERNAME" --password-stdin
docker build -t "$DOCKER_USERNAME/multi-purpose-tg-bot:$TRAVIS_TAG" .
docker push "$DOCKER_USERNAME/multi-purpose-tg-bot:$TRAVIS_TAG"