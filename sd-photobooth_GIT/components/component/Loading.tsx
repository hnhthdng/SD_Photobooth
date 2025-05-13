"use client"
import React from 'react'
// import { Progress } from "@/components/ui/progress"

const Loading = () => {
  return (
    <div className="fixed inset-0 flex items-center justify-center">
      <div className="relative flex items-center justify-center">
        <div className="size-12 rounded-full border-4 border-neutral-700 border-t-transparent dark:invert animate-spinScale" />
        <div className="absolute size-9 rounded-full border-4 border-neutral-700 border-b-transparent dark:invert animate-spinScaleReverse" />
        <span className="sr-only">Loading...</span>
      </div>
    </div>


  )
}

export default Loading
