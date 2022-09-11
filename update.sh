#replace ips from domains.json and ssl_env by the host ip
ip=$(wget -qO - ifconfig.me/ip)
sed -i "s/\([0-9]\{1,3\}.\)\{3\}[0-9]\{1,3\}/$ip/" domains.json 
sed -i "s/\([0-9]\{1,3\}.\)\{3\}[0-9]\{1,3\}/$ip/" substitute.env
sed -i "s/\([0-9]\{1,3\}.\)\{3\}[0-9]\{1,3\}/$ip/" .env
docker compose pull
docker compose up -d --force-recreate
