up: up-db restart-api restart-console-client

restart-api:
	docker compose -f docker-compose.api.yml up -d --force-recreate

restart-console-client:
	docker compose -f docker-compose.client.yml up -d --force-recreate

up-db:
	docker compose -f docker-compose.database.yml up -d --force-recreate
