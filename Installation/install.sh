TEMP_DEB="$(mktemp)" &&
echo "HELOOOOOO $TEMP_DB" &&
wget -O "$TEMP_DEB" 'https://raw.githubusercontent.com/SparkyCoder/Gpio_Controller_Api/main/Installation/gpio-controller-api-1.0.deb' &&
sudo dpkg -i "$TEMP_DEB"
rm -f "$TEMP_DEB"

