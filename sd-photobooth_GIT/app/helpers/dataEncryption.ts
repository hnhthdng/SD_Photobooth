const encrypt = async (text: string) => {
  const encoder = new TextEncoder();
  const key = encoder.encode(process.env.ENCRYPTION_KEY);
  const iv = encoder.encode(process.env.ENCRYPTION_IV);

  const cryptoKey = await crypto.subtle.importKey(
    "raw",
    key,
    { name: "AES-CBC" },
    false,
    ["encrypt"]
  );

  const data = encoder.encode(text);

  const encrypted = await crypto.subtle.encrypt(
    { name: "AES-CBC", iv },
    cryptoKey,
    data
  );

  return Buffer.from(encrypted).toString("hex");
};

const decrypt = async (text: string) => {
  const encoder = new TextEncoder();
  const decoder = new TextDecoder();

  const key = encoder.encode(process.env.ENCRYPTION_KEY);
  const iv = encoder.encode(process.env.ENCRYPTION_IV);

  if (!key || !iv) {
    throw new Error(
      "Encryption key or IV is not set in environment variables."
    );
  }

  const cryptoKey = await crypto.subtle.importKey(
    "raw",
    key,
    { name: "AES-CBC" },
    false,
    ["decrypt"]
  );

  const encryptedData = Buffer.from(text, "hex");

  const decryptedData = await crypto.subtle.decrypt(
    { name: "AES-CBC", iv },
    cryptoKey,
    encryptedData
  );

  return decoder.decode(decryptedData);
};

export { encrypt, decrypt };
