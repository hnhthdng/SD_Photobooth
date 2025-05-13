import { z } from "zod";

export const StickerStyleSchema = z.object({
     stickerStyleName: z.string(),
     description: z.string(),
})
