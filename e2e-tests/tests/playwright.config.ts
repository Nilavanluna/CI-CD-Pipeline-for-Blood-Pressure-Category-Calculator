import { defineConfig } from '@playwright/test'

export default defineConfig({
  testDir: './tests',
  timeout: 30000,
  retries: 2,
  use: {
    baseURL: process.env.APP_URL || 'http://localhost:5000',
    headless: true,
    screenshot: 'only-on-failure',
  },
  reporter: [['html', { outputFolder: 'playwright-report' }], ['list']],
})
