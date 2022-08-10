#replace ips from domains.json and ssl_env by the host ip
sed -i "s/\([0-9]\{1,3\}.\)\{3\}[0-9]\{1,3\}/$(wget -qO - ifconfig.me/ip)/" domains.json
sed -i "s/\([0-9]\{1,3\}.\)\{3\}[0-9]\{1,3\}/$(wget -qO - ifconfig.me/ip)/" ssl_env
docker compose pull
docker compose up -d --force-recreate
